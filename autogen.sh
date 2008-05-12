#!/bin/sh

autoreconf  -i --force --warnings=none
./configure --enable-maintainer-mode $*
