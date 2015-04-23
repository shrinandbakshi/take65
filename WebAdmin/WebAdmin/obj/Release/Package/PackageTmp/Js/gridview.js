function HighlightRow(chkB) {
    var IsChecked = chkB.checked;
    if (IsChecked) {
        chkB.parentNode.parentNode.style.backgroundColor = '#f2f2f2';
        chkB.parentNode.parentNode.style.color = '#666';
    } else {
        chkB.parentNode.parentNode.style.backgroundColor = '#fafafa';
        chkB.parentNode.parentNode.style.color = '#666';
    }
}

function SelectAllCheckboxes(spanChk) {
    var oItem = spanChk.children;
    var theBox = (spanChk.type == "checkbox") ? spanChk : spanChk.children.item[0];
    xState = theBox.checked;
    elm = theBox.form.elements;
    for (i = 0; i < elm.length; i++)
        if (elm[i].type == "checkbox" && elm[i].id != theBox.id) {
            if (elm[i].checked != xState)
                elm[i].click();
        }
}
