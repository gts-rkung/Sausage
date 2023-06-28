#!/bin/bash
FILES="s1 s3 s5 s6 s8"

mkdir "iphone 6.7 1290x2796" "iphone 6.5 1242x2688" "iphone 5.5 1242x2208" "ipad 12.9 2048x2732"

for f in $FILES
do
  #break
  convert "${f}.jpg" -scale 1290x2796! "iphone 6.7 1290x2796/${f}_1290x2796.jpg"
done

for f in $FILES
do
  #break
  convert "${f}.jpg" -scale 1242x2688! "iphone 6.5 1242x2688/${f}_1242x2688.jpg"
done

for f in $FILES
do
  #break
  convert "${f}.jpg" -scale 1242x2688! tmp.jpg
  convert tmp.jpg -crop 1242x2208+0+240 "iphone 5.5 1242x2208/${f}_1242x2208.jpg"
  rm tmp.jpg
done

for f in $FILES
do
  convert "${f}.jpg" -scale 2048x4432! tmp.jpg
  convert tmp.jpg -crop 2048x2732+0+1200 "ipad 12.9 2048x2732/${f}_2048x2732.jpg"
  rm tmp.jpg
done
