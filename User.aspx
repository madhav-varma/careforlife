<%@ Page Language="C#" AutoEventWireup="true" CodeFile="User.aspx.cs" Inherits="User" MasterPageFile="~/Care4LifeMaster.master" %>
<%@ MasterType VirtualPath="~/Care4LifeMaster.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPageJs" runat="server">
     <script src="Scripts/user.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="right_col" role="main">
        <div class="">
            <div id="exTab1" class="container">
                
                    <div id="userlist">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <table id="userlist_table" class="table table-striped table-bordered" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Email Id</th>
                                                    <th>Mobile</th>
                                                    <th>Profession</th>                                                    
                                                    <th>City</th>
                                                    <th>State</th>
                                                    <th>Learning And Sharing</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th style="width: 30%">Name</th>
                                                    <th style="width: 10%">Email Id</th>
                                                    <th style="width: 10%">Mobile</th>
                                                    <th style="width: 10%">Profession</th>
                                                    <th style="width: 10%">City</th>
                                                    <th style="width: 10%">State</th>
                                                    <th></th>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>                    
                </div>            
        </div>
    </div>
</asp:Content>
