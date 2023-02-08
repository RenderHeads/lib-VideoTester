# lib-VideoTester
A little Tool to make it easier for us to check videos against some set of configurations

# development methodology
  - build out the login in the library folder and the tests folder, using a test first approach
  - when we have got all the logic out with a reasonable API, build out the console app
  - needs to work cross platform as this will end up being used by people and possibly on a server
  
# General Idea
Use FFMMPEG to query video and just against a set of configurations (probably defined by json),  if a configuration is valid for that file, allow it through otherwise print an error.   Save the results in a meta file next to the media file.

# Contribution
If you want to PR code please ensure all code is testable, but using a test first approach (write a test, see it fail, make it pass).
