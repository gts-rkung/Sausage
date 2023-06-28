#!/bin/bash
FILES="s1 s3 s5 s6 s8"

for f in $FILES
do
  #break
  convert "${f}.jpg" -crop 1080x1920+0+240! ${f}_1080x1920.jpg
done
