<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Yahoo.aspx.cs" Inherits="Website.Prototype.Wagner.Yahoo" %>

<!DOCTYPE html PUBLIC "-//W3C//DTD XHTML 1.0 Transitional//EN" "http://www.w3.org/TR/xhtml1/DTD/xhtml1-transitional.dtd">

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type='text/javascript'>
        // Base URI for Web service  
        var yql_base_uri = "http://query.yahooapis.com/v1/yql";

        // Create a variable to make results available  
        // in the global namespace  
        var yql_results = "";

        // Create a YQL query to get geo data for the  
        // San Francisco International Airport  
        var yql_query = "SELECT * from geo.places WHERE text='SFO'";

        // Callback function for handling response data  
        function handler(rsp) {
            if (rsp.data) {
                yql_results = rsp.data;
            }
        }  
    </script>  
</head>
<body>
     <!-- Div tag for stories results -->  
    <div id='results'></div>  
    <!-- The YQL statment will be assigned to src. -->  
    <script src='http://query.yahooapis.com/v1/public/yql?q=select%20*%20from%20rss%20where%20url%3D%22http%3A%2F%2Frss.news.yahoo.com%2Frss%2Ftopstories%22&format=json&callback=cbfunc'></script>   
</body>
</html>
