<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Speciality.aspx.cs" Inherits="Speciality" MasterPageFile="~/Care4LifeMaster.master" %>

<%@ MasterType VirtualPath="~/Care4LifeMaster.Master" %>
<asp:Content ID="Content1" ContentPlaceHolderID="cphPageJs" runat="server">
    <script src="Scripts/speciality.js"></script>
</asp:Content>
<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="right_col" role="main">
        <div class="">
            <div id="exTab1" class="container">
                <ul class="nav nav-tabs">
                    <li id="spleditli" class="active"><a href="#spledit" data-toggle="tab">Add/Update</a>
                    </li>
                    <li id="spllistli">
                        <a href="#spllist" data-toggle="tab">List</a>
                    </li>
                </ul>

                <div class="tab-content clearfix">
                    <div class="tab-pane" id="spllist">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <table id="spllist_table" class="table table-striped table-bordered" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>Name</th>
                                                    <th>Is Rare</th>

                                                    <th>Actions</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th style="width: 40%">Name</th>
                                                    <th style="width: 40%">Is Rare</th>

                                                    <th></th>
                                                </tr>
                                            </tfoot>
                                        </table>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="tab-pane active" id="spledit">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <form runat="server" id="splform" method="post" enctype="multipart/form-data">
                                            <input type="hidden" runat="server" id="speciality_id">
                                            <div class="row item form-group">
                                                <span class="section" style="padding-bottom: 10px">Basic Info
                                                    <button runat="server" id="sendSPL" type="submit" onserverclick="SubmitSpeciality" class="hidden pull-right btn btn-success">Save</button>
                                                    <input id="cancel" type="button" class="pull-right btn btn-danger" value="Cancel" />
                                                    <input id="saveSPL" type="button" class="pull-right btn btn-success" value="Save" />
                                                </span>

                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="name">
                                                    Speciality Name <span class="required">*</span>
                                                </label>

                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input runat="server" id="name" class="form-control col-md-7 col-xs-12" name="name" placeholder="Speciality Name" required="required" type="text">
                                                </div>
                                            </div>

                                            <div class="row item ">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="number">
                                                    Image
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12" id="image-container">
                                                    <input type="file" id="image" class="file" name="image" />
                                                </div>
                                            </div>

                                            <input type="checkbox" runat="server" id="chk_rare" class="hidden" />
                                            <div class="row form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12">Rare</label>
                                                <div class="col-md-9 col-sm-9 col-xs-12">
                                                    <div class="">
                                                        <label>
                                                            <input type="checkbox" class="js-switch" id="is_rare" checked />

                                                        </label>
                                                    </div>
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
                        <input type="hidden" id="spl_id" />
                    </form>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-secondary" data-dismiss="modal">Close</button>
                    <button type="button" class="btn btn-primary" id="uploadImages">Save changes</button>
                </div>
            </div>
        </div>
    </div>
    <input type="hidden" runat="server" id="action" />
</asp:Content>
