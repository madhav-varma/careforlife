using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Services;
using System.Web.Services;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class City : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        SessionManager.ValidateSession(Session, Response);
        var user = (UserModel)Session["user"];

        this.Master.UsernameHead = user.FullName;
        this.Master.UsernameDD = user.FullName;

        var states = new MasterDataManager().GetAvailableStates();
        foreach (var s in states)
        {
            state.Items.Add(new ListItem(s.Value, s.Id));
        }
    }
    protected void SubmitCity(object sender, EventArgs e)
    {
        try
        {
            var city = new CityModel();

            city.Name = city_name.Value;
            city.StateId = int.Parse(state.Value);
            city.DoctorCount = doc_count.Value;

            city.IsActive = true;
            city.Created = DateTime.UtcNow.AddHours(5).AddMinutes(30);

            var sqlQuery = new Helper().GetInsertQuery<CityModel>(city);
            if (!string.IsNullOrWhiteSpace(city_id.Value))
            {
                city.Id = int.Parse(city_id.Value);
                sqlQuery = new Helper().GetUpdateQuery<CityModel>(city);
            }

            var dam = new DataAccessManager().ExecuteInsertUpdateQuery(sqlQuery);
            if (dam)
            {
                Response.Redirect("City", true);
            }
        }
        catch (Exception ex)
        {
            action.Value = "Failed To Add City!";
        }
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static object GetCities(DataTableAjaxPostModel model)
    {
        var cols = new List<string>() { "LOWER(TRIM(cm.city_name))", "LOWER(TRIM(sm.state_name))", "LOWER(TRIM(cm.doc_count))" };
        // Initialization.    
        DataTableData<CityModel> result = new DataTableData<CityModel>();
        try
        {
            // Initialization.                
            string draw = model.draw.ToString();
            int startRec = model.start;
            int pageSize = model.length;

            var c_order = "";
            foreach (var o in model.order)
            {
                var columnName = cols[o.column];
                c_order += string.IsNullOrWhiteSpace(c_order) ? columnName + " " + o.dir : ", " + columnName + " " + o.dir;

            }
            if (!string.IsNullOrWhiteSpace(c_order))
            {
                c_order = " order by " + c_order;
            }

            var c_search = "";
            foreach (var s in model.columns)
            {
                if (!string.IsNullOrWhiteSpace(s.search.value) && s.searchable)
                {
                    var i = model.columns.IndexOf(s);
                    var columnName = cols[i];
                    c_search += i == 1 ? " and " + columnName + " like '%" + s.search.value.Trim().ToLower() + "%'" : " and " + columnName + " like '%" + s.search.value + "%'";
                }
            }
            var cities = new CityManager().GetAllCitiesPaginated(startRec, pageSize, c_order, c_search);

            var cityList = cities.Data;
            foreach (var city in cityList)
            {
                city.Link = "<a href='javascript:void(0);' style='margin-right:10px' class='edit-city' data-id='" + city.Id + "'>Edit</a><a href='javascript:void(0);' style='margin-left:10px' class='delete-city' data-id='" + city.Id + "'>Delete</a>";
            }

            int recFilter = cities.Data.Count;

            result.draw = Convert.ToInt32(draw);
            result.recordsTotal = cities.TotalCount;
            result.recordsFiltered = cities.TotalCount;
            result.data = cityList;
        }
        catch (Exception ex)
        {
            // Info    
            Console.Write(ex);
        }
        // Return info.    
        return result;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object GetCityById(string id)
    {
        var city = new CityManager().GetCityById(id);
        return city;
    }

    [WebMethod(EnableSession = true)]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json, UseHttpGet = true)]
    public static object DeleteCityById(string id)
    {
        var resp = new JsonResponse() { IsSuccess = true, Message = "City Deleted Successfully." };
        var canDelete = new CityManager().CanDelete(id);
        if (!canDelete)
        {
            resp.IsSuccess = false;
            resp.Message = "There are some records linked to this city, so you cannot delete this city without deleting them.";
        }
        else
        {
            resp.IsSuccess = new CityManager().DeleteCity(id);
        }

        return resp;
    }
}