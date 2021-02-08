# AxwareERC

Small application to read the Edmonton Rally Club rallycross timing results and display on a second monitor (or TV). This application is best displayed on a Full HD (1080p) screen: all 20 timed runs would be displayed correctly and about 40 competitors. On a 720p screen, only the first 10 timed runs and about 30 competitors can fit the screen. 

This application also calculates the event results and updates the championship points according to the RX rules:
 - Discard the slowest time for final time calculation;
 - 300 seconds time penalty for DNF, DNS or off-course. Any timed run is limited to 300s;
 - 3 points for fastest run in class;
 - One point per competitor in class;
 - Position points using the model 20, 18, 16, 14, 12, 10, 8, 7, 6, 5, 4, 3, 2, 1.

Input:
- AxWare ".rgg" file: generated after an event is created and saved, and updated every time a new time is recorded (and saved by the user using "File -> Save" or "Ctrl+S").

Output:
- Event results (.csv) file;
- Championship results (.erc) file;

Usage:
After a new event is created in Axware and the competitors list is updated, a new .rgg file will be generated within the current event's directory.

Use this application to load the ".rgg" file using the menu option File -> Open
* every time the file is updated within Axware, the application will automatically update the results preview.
* the application results preview calculates the raw time, the total time minus slowest lap, displays the fastest lap and calculate in class and overall positions. Competitors with less timed runs will receive a DNS (300.0s) to their time, until they complete the run, so the raw time/time minus slowest lap will reflect that.

When the event is done, use the menu option Results -> Generate to create a .csv file with the results. This results file can be used within Microsoft Excel or other spreadsheet application to format the results to be posted on the website, social media or so.
* the competitor(s) that has the most number of runs will be considered to have completed the event. Any other competitor with less runs will be assigned a DNS (300.0s) to its time. So, if a competitor has more runs than it was agreeded on, that run must be removed (or marked as rerun) within AxWare. Ex: competitors were supposed to have 15 timed runs, but a car got 16 due to miscommunication or gate keeping error - the last run must be voided, otherwise this application will consider that all competitors had 16 timed runs, but only one accomplished that.
* the results file contains results for the overall class, as well as the results per class.

If the results are considered final, use the menu option Championship -> Update Championship Points to update the .erc file with the championship points. This file is also comma separated file (csv) and could be opened on Microsoft Excel or any other spreadsheet application. Several messages will warn the user through the championship points update, as it is irreversible within the software. 

During the championship points update, the primary key used to identify competitors is the car number: unique car numbers are a good practice throughout the season. When a car number is found during the championship points update, it will double-check the competitor name: if the current event name matches the previous championship results, no further action is necessary. If the names are diferent, the application will warn the user, asking to either merge the results (name type, for example) or use the result on a new competitor. If the second option is used, two competitors with the same car number will be carried on for the rest of the season. 

This application does not have the hability to find competitors that run different car numbers on different events and/or different classes. If they want a unique overall/class result, they must run the same number and class the whole season. Any changes on car number/class must be done within AxWare. Results must be regenerated after that.

If this is the first event of the season, the application will create a new "championship.erc" file. From the second event on, it will ask for a copy of the previous championship results file, to update(merge) and overwrite it. A good practice is to maintain a copy of the championship results file for every single event, in case the file needs to be reverted manually at some point (results revision, vehicle class reassignment, name change, vehicle number change, etc.). Ex.: when generating results for event #4, create a new folder and copy the "championship.erc" file from the "event #3" folder to the "event #4" folder. In case the championship results file after event #4 needs to be regenerated for any reason, create a copy of "championship.erc" from folder #3 to #4 and regenerate the results over it.

If you have questions on how to use this application, or it requires any bug fix, email me at fer.saccon[at]gmail.com

Thanks.
