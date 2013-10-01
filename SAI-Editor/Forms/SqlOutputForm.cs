﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SAI_Editor.Database.Classes;
using SAI_Editor.Classes;

namespace SAI_Editor.Forms
{
    public partial class SqlOutputForm : Form
    {
        public SqlOutputForm(List<SmartScript> smartScripts)
        {
            InitializeComponent();
            ExportSqlToTextbox(smartScripts);
        }

        public void SqlOutputForm_Load(object sender, EventArgs e)
        {

        }

        private async void ExportSqlToTextbox(List<SmartScript> smartScripts)
        {
            if (smartScripts.Count == 0)
                return;

            string sourceName = String.Empty;

            if (smartScripts[0].entryorguid < 0)
                sourceName = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureNameByGuid(-XConverter.TryParseStringToInt32(smartScripts[0].entryorguid));
            else
                sourceName = await SAI_Editor_Manager.Instance.worldDatabase.GetCreatureNameById(XConverter.TryParseStringToInt32(smartScripts[0].entryorguid));

            richTextBoxSqlOutput.Text += "-- " + sourceName + " SAI\n";
            richTextBoxSqlOutput.Text += "SET @ENTRY := " + smartScripts[0].entryorguid + ";\n";
            richTextBoxSqlOutput.Text += "UPDATE `creature_template` SET `AIName`=" + '"' + "SmartAI" + '"' + " WHERE `entry`=@ENTRY;\n";
            richTextBoxSqlOutput.Text += "DELETE FROM `smart_scripts` WHERE `entryorguid`=@ENTRY AND `source_type`=" + smartScripts[0].source_type + ";\n";
            richTextBoxSqlOutput.Text += "INSERT INTO `smart_scripts` (`entryorguid`,`source_type`,`id`,`link`,`event_type`,`event_phase_mask`,`event_chance`,`event_flags`,`event_param1`,`event_param2`,`event_param3`,`event_param4`,`action_type`,`action_param1`,`action_param2`,`action_param3`,`action_param4`,`action_param5`,`action_param6`,`target_type`,`target_param1`,`target_param2`,`target_param3`,`target_x`,`target_y`,`target_z`,`target_o`,`comment`) VALUES\n";

            for (int i = 0; i < smartScripts.Count; ++i)
            {
                SmartScript smartScript = smartScripts[i];

                richTextBoxSqlOutput.Text += "(@ENTRY," + smartScript.source_type + "," + smartScript.id + "," + smartScript.link + "," + smartScript.event_type + "," +
                                              smartScript.event_phase_mask + "," + smartScript.event_flags + "," + smartScript.event_chance + "," + smartScript.event_param1 + "," +
                                              smartScript.event_param2 + "," + smartScript.event_param3 + "," + smartScript.event_param4 + "," + smartScript.action_type + "," +
                                              smartScript.action_param1 + "," + smartScript.action_param2 + "," + smartScript.action_param3 + "," + smartScript.action_param4 + "," +
                                              smartScript.action_param5 + "," + smartScript.action_param6 + "," + smartScript.target_type + "," + smartScript.target_param1 + "," +
                                              smartScript.target_param2 + "," + smartScript.target_param3 + "," + smartScript.target_x + "," + smartScript.target_y + "," +
                                              smartScript.target_z + "," + smartScript.target_o + "," + '"' + smartScript.comment + '"' + ")";

                if (i == smartScripts.Count - 1)
                    richTextBoxSqlOutput.Text += ";";
                else
                    richTextBoxSqlOutput.Text += ",";

                richTextBoxSqlOutput.Text += "\n"; //! White line at end of script to make it easier to select
            }
        }
    }
}
