# AWS S3 Zip Extraction for IIS Logs

# Problem To Solve

AWS S3 buckets used for IIS logging is a pain to extract. AWS prevents downloading entire buckets as a bulk extraction process and nested folder structures within 
S3 buckets quickly become a mundane task. AWS recommendation for pulling entire buckets of data by implementing a python script, however the zip file structure 
becomes very painful to manage. Furtheremore the zip files are contained in Gzip - native linux format that is not recognized by Windows Operating Systems.

# Directory Structure (Nested Zip Extraction)

root.zip
    -Level 1
          - Level 2
              -File.zip
              -File.zip
              -File.zip
          - Level 2
              -File.zip
              -File.zip
              ...
          ...
              
    -Level 1
          - Level 2
          - Level 2
          - Level 2
          - Level 2
          ...
    ...


# Solution

This script AWSS3Zip.exe was created as a solution to navigate through this multi layered zip structure in a Directory Node Tree structure with potentially hundreds of nested 
zip files within multi layered nested folder structure. IIS logging in this case separated bucket folders by:

    years --> months --> days --> hours.

After unzipping is complete with the built in 7z Dll used as containerized dependency. Unzipped files are structured and processed into a local SQLite DB file or 
connection string to a remote SQL server where the files are processed and stored in Relational Tables for Querying and reporting structure.

# Usage

The script can be accessed via Powershell command  AWS-S3-Extract.ps1.
You would have to compile the application and build it into a bin folder using .NET Core DLL  - (Build from VS 2022) 
Taking the entire contents inside the bin folder to the directory of the produced AWSS3Zip.exe file is the full application.

You may enter the path of this location into the ENvironmental variables to access the command from powershell or CMD.
