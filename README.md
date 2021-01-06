# AxwareERC

Small application to read the Edmonton Rally Club rallycross timing results and display on a second monitor (or TV). It calculates the event results and updates the championship points according to the Rx rules.

Input:
- AxWare .rgg file, generate after a event is saved and updated every time a new time is scored.

Output:
- Event results (.csv) file;
- Championship results (.erc) file;

Usage:
After a new event is created in Axware and the competitors list is updated, a new .rgg file will be generated within the current event's directory.
Load the file using the menu option File -> Open
* every time the file is updated within Axware, the application will automatically update the results preview.
* the preview calculates the raw time, the total time minus slowest lap, displays the fastest lap and calculate in class and overall positions. Competitors with less timed runs will receive a DNS (300.0s) to their time, until they complete the run, so the raw time/ time minus slowest lap will reflect that.

When the event is done, use the menu option Results -> Generate to create a .csv file with the results. This results file can be used within Microsoft Excel or other spreadsheet application to format the results to be posted on the website, social media or so.
* the competitor that has the most number of runs will be considered to have completed the event. Any other competitor with less runs will be assigned a DNS (300.0s) to its time.
* the result file contains results for the overall class, as well as the results per class.

If the results are considered final, use the menu option Championship -> Update Championship Points to update the .erc file with the championship points. This file is also comma separated file (csv) and could be opened on Microsoft Excel or any other spreadsheet application. Several messages will warn the user through the championship points update, as it is irreversible within the software. A good practice is to maintain a copy of the championship results file for every single event, in case the file needs to be reverted manually at some point.


