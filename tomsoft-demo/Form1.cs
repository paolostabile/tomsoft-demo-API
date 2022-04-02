using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace tomsoft_demo
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        #region UI Event Handlers

        private void buttonGO_Click(object sender, EventArgs e)
        {
            if(textBoxRestURL.Text != string.Empty)
            {
                RestClient restClient = new RestClient();
                restClient.endPoint = textBoxRestURL.Text;

                if (RollYourOwn.Checked)
                {
                    restClient.authenticationMethod = authenticationMethod.RollYourOwn;
                    debugOutput("AuthTehnique: Roll Your Own");
                    debugOutput("AuthType: Basic");
                }

                else
                {
                    restClient.authenticationMethod = authenticationMethod.NetworkCredential;
                    debugOutput("AuthTehnique: NetworkCredential");
                    debugOutput("AuthType: NetworkCredential decides");
                }

                restClient.userName = textBoxUsername.Text;
                restClient.password = textBoxPassword.Text;
                debugOutput("Rest Client Created");
                string strResponse = string.Empty;
                strResponse = restClient.makeRequest();
                deserializeJSON(strResponse);
            }
            else
            {
                MessageBox.Show("Empty Request", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textBoxResponse.Text = string.Empty;
            checkBoxFilter.Checked = false;
        }

        #endregion

        #region Deserialize JSON

        private void deserializeJSON(string strJSON)
        {
            try
            {
                if (!checkBoxFilter.Checked)
                {
                    var JPerson = JsonConvert.DeserializeObject(strJSON);
                    debugOutput("JSON object: " + JPerson.ToString());
                }
                else
                {
                    JObject obj = JObject.Parse(strJSON);
                    JToken token1 = obj["result"][0]["artikli"][0]["id"];
                    debugOutput(token1.ToString());
                    JToken token2 = obj["result"][0]["artikli"][0]["naziv"];
                    debugOutput(token2.ToString());
                }
            }
            catch (Exception ex)
            {
                debugOutput("Error: " + ex.Message.ToString());
            }
        }

        #endregion

        #region Debug Output

        private void debugOutput(string strDebugText)
        {
            try
            {
                System.Diagnostics.Debug.Write(strDebugText + Environment.NewLine);
                textBoxResponse.Text = textBoxResponse.Text + strDebugText + Environment.NewLine;
                textBoxResponse.SelectionStart = textBoxResponse.TextLength;
                textBoxResponse.ScrollToCaret();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.Message, ToString() + Environment.NewLine);
            }
        }

        #endregion

    }
}
