using System;
using System.Collections.Generic;
using System.Text;

namespace Estimating.CSVHandler
{
   public class PhaseCodeHelper
   
    {
        //Synonym for PhaseCode value. This is Bluebeam's default name for the column in which the phase code from the custom toolbox appears.  
        //In case the reader is wondering, yes it is possible to create a custom column and display a "PhaseCode" header.  However, to ensure that
        //custom columns appear in the user's copy of Bluebeam is not so easy.  To accomplish this, a custom profile must be created to accompany the custom 
        //toolbox.  Then - when the user wishes to export the CSV file, he/she must remember to change Profiles and verify that the custom columns are being shown.
        //To complicate things further, Bluebeam's recent upgrades (currently on Bluebeam 2019) have been buggy and unreliable...and there have beeen problems 
        //porting profiles with custom columns and toolsets across computers.  As an aside, I have very good reasons to suspect that the problem lies in how they
        //are cacheing data during sessions, but it doesn't matter since the problems remain and are bad enough that we basically can't rely on the software if 
        //we pursue a "custom profile" solution.
        //
        //For perspective, all that trouble would be just to solve the problem of having poor naming on our CSV column headers.  Not really worth the trouble at the 
        //moment, is it?  With that in mind, I made what I think is a much better choice - keeping the core model and functionality as it is (well-named and clear) and 
        //adding a helper class to interface between the stupid, rigid names given by Bluebeam and the wonderful, simple names given by me to the application classes.
        //
        //There several advantages to this approach: 
        //      1) The Subject column almost ALWAYS appears in Bluebeam, no matter what profile is turned on.  It is more likely than not to 
        //         be turned on already. 
        //      2) In a word, decoupling.  When and if Bluebeam ever changes their default column names, the only code that needs to be updated is in this helper class;  
        //         The properties of the PhaseCode object remain as they are and the application logic that makes use of them doesn't run the risk of breaking because 
        //         something got renamed.

        public string Subject { get; set; }
        public string Space { get; set; }



    }
}
