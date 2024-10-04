@echo off
echo Cloning the ARToolkit-Unity-Template repository...

rem Clone the repository
git clone https://github.com/CursedPrograms/ARToolkit-Unity-Template.git temp_repo

echo Moving contents of Unity directory to current folder...
rem Move the contents of the Unity directory to the current folder
xcopy "temp_repo/src/Unity\*" "." /E /I /Y

echo Cleaning up temporary files...
rem Remove the cloned repository
rmdir /S /Q temp_repo

echo Download complete. The contents of the Unity directory are now in the current folder.
pause
