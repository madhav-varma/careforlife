﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Care4LifeMaster.master" CodeFile="MedicalFacility.aspx.cs" Inherits="MedicalFacility" %>
<%@ MasterType VirtualPath="~/Care4LifeMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPageJs" runat="server">
    <script src="Scripts/medicalfacility.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="right_col" role="main">
        <div class="">
            <div id="exTab1" class="container">
                <ul class="nav nav-tabs">
                    <li id="mfeditli" class="active"><a href="#mfedit" data-toggle="tab">Add/Update</a>
                    </li>
                    <li id="mflistli">
                        <a href="#mflist" data-toggle="tab">List</a>
                    </li>
                </ul>

                <div class="tab-content clearfix">
                    <div class="tab-pane" id="mflist">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <table id="mflist_table" class="table table-striped table-bordered" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Address</th>
                                                    <th>Email</th>
                                                    <th>Mobile</th>
                                                    <th>City</th>
                                                    <th>Actions</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th style="width: 35%">Name</th>
                                                    <th style="width: 15%">Address</th>
                                                    <th style="width: 15%">Email</th>
                                                    <th style="width: 10%">Mobile</th>
                                                    <th style="width: 10%">City</th>
                                                    <th></th>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane active" id="mfedit">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <form runat="server" id="mfform" method="post">
                                            <input type="hidden" runat="server" id="facility_id">
                                            <div class="row item form-group">
                                                <span class="section" style="padding-bottom: 10px">Basic Info
                                                    <button runat="server" id="sendMF" type="submit" onserverclick="SubmitMedicalFacility" class="hidden pull-right btn btn-success">Save</button><input id="saveMF" type="button" class="pull-right btn btn-success" value="Save" />
                                                </span>

                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="name">
                                                    Hospital Name <span class="required">*</span>
                                                </label>

                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input runat="server" id="name" class="form-control col-md-7 col-xs-12" name="name" placeholder="Name" required="required" type="text">
                                                </div>

                                            </div>
                                            <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="doctor">
                                                   Doctor Name <span class="required">*</span>
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input type="text" runat="server" required="required" id="doctor" name="doctor" placeholder="Doctor Name" class="form-control col-md-7 col-xs-12">
                                                </div>
                                            </div>
                                             <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12">
                                                    Description <span class="required">*</span>
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <textarea runat="server" placeholder="Description" id="description" required="required" name="description" class="form-control col-md-7 col-xs-12"></textarea>
                                                </div>
                                            </div>

                                            <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12">
                                                    Address <span class="required">*</span>
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <textarea runat="server" placeholder="Address" id="address" required="required" name="address" class="form-control col-md-7 col-xs-12"></textarea>
                                                </div>
                                            </div>
                                             <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12">
                                                    Timing <span class="required">*</span>
                                                </label>

                                                <div class="col-md-3 col-sm-3 col-xs-12">
                                                    <div class="form-group">
                                                        <div class='input-group date' style="width: 100%">
                                                            <input runat="server" required="required" id="timingFrom" name="timingFrom" placeholder="From e.g. 10 AM" type='text' class="form-control" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm-3 col-xs-12">
                                                    <div class="form-group">
                                                        <div class='input-group date' style="width: 100%">
                                                            <input runat="server" required="required" id="timingTo" name="timingTo" placeholder="To e.g. 2 PM" type='text' class="form-control" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>
                                            <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="email">
                                                    Email <span class="required">*</span>
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input type="email" runat="server" id="email" name="email" placeholder="Email" class="form-control col-md-7 col-xs-12">
                                                </div>
                                            </div>

                                            <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="number">
                                                    Mobile Number <span class="required">*</span>
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input type="tel" runat="server" data-rule-number="true" data-rule-minlength="10" data-rule-maxlength="10" id="mobile" name="mobile" required="required" placeholder="Mobile" class="form-control col-md-7 col-xs-12">
                                                </div>
                                            </div>

                                            <div class="item row form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12">City</label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <select id="city" runat="server" required="required" class="select2_group form-control">
                                                        <option value="">Select City</option>
                                                    </select>
                                                </div>
                                            </div>
                                             <div class=" row item form-group">
                                                <span class="section">Available Services                                                    
                                                    <button type="button" id="addservices" style="margin: 10px 0" class="btn btn-primary"><i class="fa fa-plus"></i></button>
                                                </span>

                                                <div id="services_rep"></div>
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

    <div class="modal fade" id="exampleModal" tabindex="-1" role="dialog" aria-labelledby="exampleModalLabel" aria-hidden="true">
        <div class="modal-dialog" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <h3 class="modal-title" id="exampleModalLabel">Upload Images
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close">
                        <span aria-hidden="true">&times;</span>
                    </button>
                    </h3>

                </div>
                <div class="modal-body">
                    <form class="dropzone" id="my-dropzone">
                        <input type="hidden" id="mf_id" />
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" id="uploadImages">Save changes</button>
                </div>
            </div>
        </div>
    </div>

     <script id="servicesTemplate" type="text/html">
        <div class="row services-div" data-index="${index}">
            <label class="col-sm-3 control-label">Services</label>
            <div class="col-sm-6 ">
                <div id="Container" class="input-group" style="width: 100%">
                    <input value="${service}" required="required" placeholder="Services" type="text" class="form-control" id="service${index}" name="service${index}">
                    {{if index > 0}}
                <span class="input-group-btn">
                    <button type="button" value="Clone it" class="del-services btn btn-danger"><i class="fa fa-minus"></i></button>
                </span>
                    {{/if}}
                </div>
            </div>
        </div>
    </script>


    <input type="hidden" runat="server" id="action" />

</asp:Content>

