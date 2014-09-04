<%@ Page Title="Home Page" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="AspNetQuartSignalR._Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div>
        <button type="button" id="check-quartz-status">Check Status</button>
        <button type="button" id="stop-quartz-execution">Stop Quartz</button>
        <br />

        <ul id="quartz-execution">
        </ul>

    </div>
</asp:Content>

<asp:Content ID="Content1" ContentPlaceHolderID="ScriptsContent" runat="server">
    <script type="text/javascript">
        $(document).ready(function () {
            var quartz = $.connection.quartzHub;

            $('#check-quartz-status').on('click', function () {
                quartz.server.checkQuartzStatus();
            });

            $('#stop-quartz-execution').on('click', function () {
                quartz.server.stopQuartzExecution();
            });

            quartz.client.quartzJobExecuted = function (message) {
                $('#quartz-execution').append('<li>' + message + '</li>');
            };

            quartz.client.onCheckQuartzStatus = function (message) {
                alert(message);
            };

            $.connection.hub.start()
                .done(function () { console.log('Now connected, connection ID=' + $.connection.hub.id); })
                .fail(function () { console.log('Could not Connect!'); });
        });
    </script>
</asp:Content>

