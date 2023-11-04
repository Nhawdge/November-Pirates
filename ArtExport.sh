#! /bin/sh

asepritePath="/d/Program\ Files/Aseprite/Aseprite.exe"
inputFolder="Assets/Aseprite"

for file in "$inputFolder"/*.ase; do
	filename=$(basename "$file")
	filenameAlone="${filename%.*}"
	arguments="-b Assets/Aseprite/$filenameAlone.ase --data Assets/Art/$filenameAlone.json --sheet Assets/Art/$filenameAlone.png --sheet-pack --format json-array --filename-format {tag}-{frame} --list-tags --extrude --color-mode rgb"
	eval "$asepritePath $arguments"
done


# /d/Program\ Files/Aseprite/Aseprite.exe -b Assets/Aseprite/hullLarge.ase --data Assets/Art/hullLarge.json --sheet Assets/Art/hullLarge.png --sheet-pack --format json-array --filename-format {tag}-{frame} --list-tags --extrude
