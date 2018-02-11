<%@ Page Language="C#" AutoEventWireup="true" CodeFile="City.aspx.cs" Inherits="City" MasterPageFile="~/Care4LifeMaster.master" %>

<%@ MasterType VirtualPath="~/Care4LifeMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPageJs" runat="server">
    <script src="Scripts/city.js"></script>
</asp:Content>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="right_col" role="main">
        <div class="">
            <div id="exTab1" class="container">
                <ul class="nav nav-tabs">
                    <li id="cityeditli" class="active"><a href="#cityedit" data-toggle="tab">Add/Update</a>
                    </li>
                    <li id="citylistli">
                        <a href="#citylist" data-toggle="tab">List</a>
                    </li>
                </ul>

                <div class="tab-content clearfix">
                    <div class="tab-pane" id="citylist">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <table id="citylist_table" class="table table-striped table-bordered" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>City Name</th>
                                                    <th>State Name</th>
                                                    <th>Doctor Count</th>

                                                    <th>Actions</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th style="width: 20%">City Name</th>
                                                    <th style="width: 60%">State Name</th>
                                                    <th style="width: 10%">Doctor Count</th>

                                                    <th></th>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane active" id="cityedit">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <form runat="server" id="cityform" method="post">
                                            <input type="hidden" runat="server" id="city_id">
                                            <div class="row item form-group">
                                                <span class="section" style="padding-bottom: 10px">Basic Info
                                                    <button runat="server" id="sendCity" type="submit" onserverclick="SubmitCity" class="hidden pull-right btn btn-success">Save</button>
                                                    <input id="cancel" type="button" class="pull-right btn btn-danger" value="Cancel" />
                                                    <input type="button" id="saveCity" class="pull-right btn btn-success" value="Save" />

                                                </span>

                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="video_name">
                                                    City Name <span class="required">*</span>
                                                </label>

                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input runat="server" id="city_name" class="form-control col-md-7 col-xs-12" name="city_name" placeholder="City Name" required="required" type="text">
                                                </div>

                                            </div>

                                            <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="number">
                                                    Doctor Count <span class="required">*</span>
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input runat="server" type="number" id="doc_count" name="doc_count" required="required" placeholder="Doctor Count" class="form-control col-md-7 col-xs-12">
                                                </div>
                                            </div>

                                            <div class="item row form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12">State</label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <select id="state" runat="server" required="required" class="select2_group form-control">
                                                        <option value="">Select State</option>
                                                    </select>
                                                </div>
                                            </div>
                                        </form>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
        </div>
    </div>

    <input type="hidden" runat="server" id="action" />

</asp:Content>

