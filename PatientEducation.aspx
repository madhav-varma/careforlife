<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Care4LifeMaster.master" CodeFile="PatientEducation.aspx.cs" Inherits="PatientEducation" %>

<%@ MasterType VirtualPath="~/Care4LifeMaster.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPageJs" runat="server">
    <script src="Scripts/patient.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="right_col" role="main">
        <div class="">
            <div id="exTab1" class="container">
                <ul class="nav nav-tabs">
                    <li id="patienteditli" class="active"><a href="#patientedit" data-toggle="tab">Add/Update</a>
                    </li>
                    <li id="patientlistli">
                        <a href="#patientlist" data-toggle="tab">List</a>
                    </li>
                </ul>

                <div class="tab-content clearfix">
                    <div class="tab-pane" id="patientlist">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <table id="patientlist_table" class="table table-striped table-bordered" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>Video Name</th>
                                                    <th>Video Url</th>
                                                    <th>Speciality</th>

                                                    <th>Actions</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th style="width: 20%">Video Name</th>
                                                    <th style="width: 60%">Video Url</th>
                                                    <th style="width: 10%">Speciality</th>

                                                    <th></th>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane active" id="patientedit">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <form runat="server" id="patientform" method="post">
                                            <input type="hidden" runat="server" id="video_id">
                                            <div class="row item form-group">
                                                <span class="section" style="padding-bottom: 10px">Basic Info
                                                    <button runat="server" id="sendPE" type="submit" onserverclick="SubmitPatientVideo" class="hidden pull-right btn btn-success">Save</button>
                                                    <input id="cancel" type="button" class="pull-right btn btn-danger" value="Cancel" />
                                                    <input type="button" id="savePE" class="pull-right btn btn-success" value="Save" />

                                                </span>

                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="video_name">
                                                    Video Name <span class="required">*</span>
                                                </label>

                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input runat="server" id="video_name" class="form-control col-md-7 col-xs-12" name="video_name" placeholder="Video Name" required="required" type="text">
                                                </div>

                                            </div>
                                            <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="video_url">
                                                    Video Url <span class="required">*</span>
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input runat="server" id="video_url" class="form-control col-md-7 col-xs-12" name="video_url" placeholder="Video Url" required="required" type="text">
                                                </div>
                                            </div>

                                            <div class="item row form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12">Speciality</label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <select id="speciality" runat="server" required="required" class="select2_group form-control">
                                                        <option value="">Select Speciality</option>
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

