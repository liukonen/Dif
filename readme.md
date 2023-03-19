# About
This program was created out of a need to see what changes could happen if I ran a robocopy command between 2 directories. 

_updated 2023-03-18_
I didn't know at the time, there was a list option, (/l) so instead I built a small utility to list out the differences of two directories, based on if the file does not exist, or if the files timestamps are different.
While there a number of different ways to compare files, I chose to use the LastModifided timestamp of the file for distinction.


# Learnings
* Parallel tasks
* File enumeration
