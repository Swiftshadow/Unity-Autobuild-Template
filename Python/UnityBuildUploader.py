import sys
import os
import shutil
from pydrive.auth import GoogleAuth
from pydrive.drive import GoogleDrive
from datetime import date

# Take the folder at the given path and upload it to a folder in a team drive
def main(folderPath, folderId, teamDriveId):


    # Get the auth tokens for the drive
    gauth = GoogleAuth()
    #gauth.LoadCredentialsFile("../../../conf/token.json")
    gauth.LocalWebserverAuth()

    drive = GoogleDrive(gauth)

    # Get all the files currently in the folder. Used for autoincrementing the version number
    files = drive.ListFile(
        {
            'q': "'{}' in parents and trashed=false".format(folderId),
            'includeItemsFromAllDrives': True,
            'supportsTeamDrives': True
        }).GetList()
    # Attempt to split the name on "V" and increment the version number
    today = date.today()
    try:
        oldName = files[0]['title']
        print(oldName)
        splitName = oldName.split("V")
        # If this is the first build of the day, intentionally throw an error to restart the count
        if splitName[0] != today.strftime("%m-%d-%Y"):
            newName = files[len(files) + 1]
        versionNumber = int(splitName[1]) + 1
        #newName = folderPath + "v" + str(versionNumber)
        newName = "{}V{}".format(today.strftime("%m-%d-%Y"), str(versionNumber))
    except IndexError:
        # If the split cannot be done, assume no file and upload V1
        newName = "{}V1".format(today.strftime("%m-%d-%Y"))

    # Set up the metadata for the file
    file = drive.CreateFile({
        'title': newName,
        'parents': [{
            'teamDriveId': teamDriveId,
            'id': folderId
        }],
        'description': "UNITY_AUTOBUILD_UPLOADED",
    })

    # Zip up the builds folder
    #with ZipFile('Build.zip', 'w') as zip:
    #    for folderName, subfolders, filenames in os.walk(folderPath):
    #        for filename in filenames:
    #            filePath = os.path.join(folderName, filename)
    #            zip.write(filePath)

    shutil.make_archive("Build", 'zip', "../BuildOutput")
    
    # Upload the file to drive
    file.SetContentFile('Build.zip')
    file.Upload(param={'supportsTeamDrives': True})

# Ensure the program is being used correctly and with the correct parameters
if __name__ == '__main__':
    if len(sys.argv) != 4:
        sys.exit("Wrong parameters, proper usage is: python3 main.py [filepath] [folderId] [teamDriveId]")

    main(sys.argv[1], sys.argv[2], sys.argv[3])