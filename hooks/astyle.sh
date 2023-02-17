#! /bin/bash

git diff --cached --name-only | grep -E "\.cs$" | xargs astyle -n
