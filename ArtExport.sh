#! /bin/sh

/d/Program\ Files/Aseprite/Aseprite.exe -b Assets/Aseprite/hullLarge.ase --data Assets/Art/hullLarge.json --sheet Assets/Art/hullLarge.png --sheet-pack --format json-array --filename-format {tag}-{frame} --list-tags --extrude
/d/Program\ Files/Aseprite/Aseprite.exe -b Assets/Aseprite/mainFlag.ase --data Assets/Art/mainFlag.json --sheet Assets/Art/mainFlag.png --sheet-pack --format json-array --filename-format {tag}-{frame} --list-tags --extrude
