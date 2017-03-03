@echo off

for /f "tokens=2 delims==" %%a in ('wmic OS Get localdatetime /value') do set "dt=%%a"
set "YY=%dt:~2,2%" & set "YYYY=%dt:~0,4%" & set "MM=%dt:~4,2%" & set "DD=%dt:~6,2%"
set "DATESTAMP=%YYYY%-%MM%-%DD%"
for /f %%i in ('git rev-list HEAD --count') do (set REVISION=%%i)
echo REV.%REVISION% %DATESTAMP%

echo VERSTR:	.ASCIZ /REV.%REVISION% %DATESTAMP%/ > VERSIO.MAC
echo	.EVEN >> VERSIO.MAC

@echo on
@if exist TILES.OBJ del TILES.OBJ
@if exist EXPRES.LST del EXPRES.LST
@if exist EXPRES.OBJ del EXPRES.OBJ
C:\bin\rt11\rt11.exe MACRO/LIST:DK: EXPRES.MAC+SYSMAC.SML/LIBRARY
