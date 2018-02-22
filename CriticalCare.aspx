<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Care4LifeMaster.master" CodeFile="CriticalCare.aspx.cs" Inherits="CriticalCare" %>

<%@ MasterType VirtualPath="~/Care4LifeMaster.Master" %>

<asp:Content ID="Content1" ContentPlaceHolderID="cphPageJs" runat="server">
    <script src="Scripts/criticalcare.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="right_col" role="main">
        <div class="">


            <div id="exTab1" class="container">
                <ul class="nav nav-tabs">
                    <li id="cceditli" class="active"><a href="#ccedit" data-toggle="tab">Add/Update</a>
                    </li>
                    <li id="cclistli">
                        <a href="#cclist" data-toggle="tab">List</a>
                    </li>
                </ul>

                <div class="tab-content clearfix">
                    <div class="tab-pane" id="cclist">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <table id="cclist_table" class="table table-striped table-bordered" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Email</th>
                                                    <th>Mobile</th>
                                                    <th>City</th>
                                                    <th>Actions</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th style="width: 35%">Name</th>
                                                    <th style="width: 15%">Email</th>
                                                    <th style="width: 15%">Mobile</th>
                                                    <th style="width: 15%">City</th>
                                                    <th></th>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane active" id="ccedit">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <form runat="server" id="ccform" method="post" enctype="multipart/form-data">
                                            <input type="hidden" runat="server" id="cc_id">
                                            <div class="row item form-group">
                                                <span class="section" style="padding-bottom: 10px">Basic Info
                                                    <button runat="server" id="sendCC" type="submit" onserverclick="SubmitCriticalCare" class="hidden pull-right btn btn-success">Save</button>
                                                    <input id="cancel" type="button" class="pull-right btn btn-danger" value="Cancel" />
                                                    <input id="saveCC" type="button" class="pull-right btn btn-success" value="Save" />
                                                </span>

                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="name">
                                                    Hospital Name <span class="required">*</span>
                                                </label>

                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input runat="server" id="name" class="form-control col-md-7 col-xs-12" name="name" placeholder="Name" required="required" type="text">
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
                                                    <input type="tel" data-rule-number="true" data-rule-minlength="10" data-rule-maxlength="12" runat="server" id="mobile" name="mobile" required="required" placeholder="Mobile" class="form-control col-md-7 col-xs-12">
                                                </div>
                                            </div>

                                            <div class="row item ">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="number">
                                                    Images 
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12" id="image-container">
                                                    <input type="file" id="images" multiple="multiple" class="file" name="images[]"/>
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

                                                <label class="col-md-3 col-sm-3 col-xs-12 control-label">Specialities</label>
                                                <div class="col-md-6 col-sm-6 col-xs-12 ">

                                                    <textarea rows="4" required="required" runat="server" placeholder="Speciality" class="form-control" id="speciality" name="speciality"></textarea>

                                                </div>
                                            </div>

                                            <div class=" row item form-group">
                                                <span class="section">Available Services                                                    
                                                    <button type="button" id="addservices" style="margin: 10px 0" class="btn btn-primary"><i class="fa fa-plus"></i></button>
                                                </span>

                                                <div id="cc_services_rep"></div>
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
                        <input type="hidden" id="cc_dz_id" />
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
                <div class="input-group" style="width: 100%">
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

    <script id="specialitiesTemplate" type="text/html">
        <div class="row specialities-div" data-index="${index}">
            <label class="col-sm-3 control-label">Speciality</label>
            <div class="col-sm-6 ">
                <div class="input-group" style="width: 100%">
                    <input value="${speciality}" required="required" placeholder="Speciality" type="text" class="form-control" id="speciality${index}" name="speciality${index}">
                    {{if index > 0}}
                <span class="input-group-btn">
                    <button type="button" value="Clone it" class="del-specialities btn btn-danger"><i class="fa fa-minus"></i></button>
                </span>
                    {{/if}}
                </div>
            </div>
        </div>
    </script>

    <input type="hidden" runat="server" id="action" />

</asp:Content>
