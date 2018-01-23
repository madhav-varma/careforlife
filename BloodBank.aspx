<%@ Page Language="C#" MasterPageFile="~/Care4LifeMaster.Master" AutoEventWireup="true" CodeFile="BloodBank.aspx.cs" Inherits="BloodBank" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

    <div class="right_col" role="main">
        <div class="">


            <div id="exTab1" class="container">
                <ul class="nav nav-tabs">
                    <li id="bloodbankeditli" class="active"><a href="#bloodbanklabedit" data-toggle="tab">Add/Update</a>
                    </li>
                    <li id="bloodbanklistli">
                        <a href="#bloodbanklist" data-toggle="tab">List</a>
                    </li>
                </ul>

                <div class="tab-content clearfix">
                    <div class="tab-pane" id="bloodbanklist">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <table id="bloodblanklist_table" class="table table-striped table-bordered" style="width: 100%">
                                            <thead>
                                                <tr>
                                                    <th>BloodBank Name</th>
                                                    <th>Email Id</th>
                                                    <th>Mobile</th>
                                                    <th>Year Of Opening</th>
                                                    <th>Timing</th>
                                                    <th>City</th>
                                                    <th>Actions</th>
                                                </tr>
                                            </thead>
                                            <tfoot>
                                                <tr>
                                                    <th style="width: 10%">BloodBank Name</th>
                                                    <th>Email Id</th>
                                                    <th style="width: 25%">Mobile</th>
                                                    <th>Year Of Opening</th>
                                                    <th>Timing</th>
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
                    <div class="tab-pane active" id="bloodbankedit">
                        <div class="row">
                            <div class="col-md-12 col-sm-12 col-xs-12">
                                <div class="x_panel" style="border-top: 0">
                                    <div class="x_content">
                                        <form runat="server" id="bloodbankform" method="post">
                                            <input type="hidden" runat="server" id="blood_bank_id">
                                            <div class="row item form-group">
                                                <span class="section" style="padding-bottom: 10px">Basic Info
                                                    <button runat="server" id="send" type="submit" onserverclick="SubmitBloodBank" class="pull-right btn btn-success">Save</button>
                                                    
                                                </span>

                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="lab_name">
                                                    BloodBank Name <span class="required">*</span>
                                                </label>

                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input runat="server" id="blood_bank_name" class="form-control col-md-7 col-xs-12" name="lab_name" placeholder="Lab Name" required="required" type="text">
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
                                                            <input runat="server" id="timingFrom" name="timingFrom" placeholder="From e.g. 10 AM" type='text' class="form-control" />
                                                        </div>
                                                    </div>
                                                </div>
                                                <div class="col-md-3 col-sm-3 col-xs-12">
                                                    <div class="form-group">
                                                        <div class='input-group date' style="width: 100%">
                                                            <input runat="server" id="timingTo" name="timingTo" placeholder="To e.g. 2 PM" type='text' class="form-control" />
                                                        </div>
                                                    </div>
                                                </div>
                                            </div>

                                            <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="opening_year">
                                                    Year Of Opening <span class="required">*</span>
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input runat="server" id="opening_year" name="opening_year" required="required" placeholder="eg. 1996" class="form-control col-md-7 col-xs-12">
                                                </div>
                                            </div>

                                            <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="email">
                                                    Email <span class="required">*</span>
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input type="email" runat="server" id="email" name="email" placeholder="Email" required="required" class="form-control col-md-7 col-xs-12">
                                                </div>
                                            </div>

                                            <div class="row item form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12" for="number">
                                                    Mobile Number <span class="required">*</span>
                                                </label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <input type="tel" runat="server" id="mobile" name="mobile" required="required" placeholder="Mobile" class="form-control col-md-7 col-xs-12">
                                                </div>
                                            </div>


                                           


                                            <div class="item row form-group">
                                                <label class="control-label col-md-3 col-sm-3 col-xs-12">City</label>
                                                <div class="col-md-6 col-sm-6 col-xs-12">
                                                    <select id="city" runat="server" required="required" class="select2_group form-control">
                                                        <option>Select City</option>
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
                        <input type="hidden" id="pl_id" />
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
