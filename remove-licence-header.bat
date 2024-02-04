@echo off
setlocal enabledelayedexpansion

for /r %%i in (*.cs) do (
    set "remove=0"
    (
        for /f "delims=" %%j in (%%i) do (
            if !remove! == 1 (
                echo %%j
            )
            if "%%j" == "// ---------------------------------------------------------------------------" (
                set "remove=1"
                echo %%j
            )
        )
    ) > temp_file
    move /y temp_file "%%i"
)

endlocal
