#!/bin/bash

# Crop
WIDTH=1080
HEIGHT=1350
OFFSET=525
QUALITY=19 # default 23, lower number means better quality
FILES="win4 win5 lose5"
# please export files as xxx_adobe.mp4 in adobe premiere 
for f in $FILES
do
  f=$(basename $f .mp4)
  ffmpeg -i "${f}_adobe.mp4" -vf crop=$WIDTH:$HEIGHT:0:$OFFSET -c:v libx264 -crf $QUALITY -c:a aac "${f}_${WIDTH}x${HEIGHT}.mp4"
done

WIDTH=1080
HEIGHT=1920
OFFSET=240
for f in $FILES
do
  f=$(basename $f .mp4)
  ffmpeg -i "${f}_adobe.mp4" -vf crop=$WIDTH:$HEIGHT:0:$OFFSET -c:v libx264 -crf $QUALITY -c:a aac "${f}_${WIDTH}x${HEIGHT}.mp4"
done

# Scale
WIDTH=800 #1024
HEIGHT=1000 #1280
for f in $FILES
do
  ffmpeg -i "${f}_1080x1350.mp4" -vf scale=$WIDTH:$HEIGHT: -c:a copy "${f}_${WIDTH}x${HEIGHT}.mp4"
done

WIDTH=720
HEIGHT=1280
for f in $FILES
do
  ffmpeg -i "${f}_1080x1920.mp4" -vf scale=$WIDTH:$HEIGHT: -c:a copy "${f}_${WIDTH}x${HEIGHT}.mp4"
done
