using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class Care4LifeMaster : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    public string UsernameHead
    {
        get
        {
            return username_head.InnerText;
        }
        set
        {
            username_head.InnerText = value;
        }
    }
    public string UsernameDD
    {
        get
        {
            return username_dd.InnerText;
        }
        set
        {
            username_dd.InnerText = value;
        }
    }
}
