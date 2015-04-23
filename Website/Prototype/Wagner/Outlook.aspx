<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="OutlookCallback.aspx.cs" Inherits="Website.Prototype.OutlookCallback" %>

<!DOCTYPE html>
<html>
<head>
        <script src="http://js.live.net/v5.0/wl.js"></script>
</head>
<body>

<div id="signin"></div>

<script>

    // Listen for login events
    // Place this before WL.init so we may trigger it on page refresh too.
    WL.Event.subscribe("auth.login", getContact);

    // Load the app
    WL.init({
        client_id: "000000004810884A",
        redirect_uri: "http://take65-v2.local.netbiis.com/Prototype/Wanger/Outlook.aspx"
    });

    function error(text) {
        alert(text);
        return '{"status":false,"response":"Error: ' + text + '"}';
    }

    // Get contacts
    function getContact(e) {

        // Do we have permission to collect emails
        if (!WL.getSession()) {
            error("Please signin");
            return;
        }
        else if (WL.getSession().scope.indexOf("wl.contacts_emails") === -1) {
            error("Please update your permissions");
            return;
        }

        WL.api("/me/contacts", function (r) {
            if (r.error) {
                error("You don't have permissions");
                return;
            }
            else {

                var json = '[';


                // loop through and print out each key=>value pair.
                for (var i = 0; i < r.data.length; i++) {
                    if (r.data[i].emails.preferred) {
                        json += '{';
                        json += '"name": "' + r.data[i].name + '",';
                        json += '"email": "' + r.data[i].emails.preferred + '",';
                        json += '"image": null';

                        if (i == (r.data.length - 1)) {
                            json += '}';
                        } else {
                            json += '},';
                        }
                    }
                }

                json += ']';
            }

            console.log(json);

            return json;


        });
    };

    WL.ui({ name: "signin", element: "signin", scope: "wl.basic, wl.signin, wl.contacts_emails" });
</script>
</body>
</html>