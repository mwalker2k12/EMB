<%@ Control Language="C#" AutoEventWireup="true" CodeFile="WiControl.ascx.cs" Inherits="CustomControls_WiControl" %>
   <script language="javascript" type="text/javascript">
     function wiHideShow(divId,linkId)
         {
            var obj = document.getElementById("<%=this.ClientID%>_" + divId);
            var obj2 = document.getElementById(linkId);
            
            if(obj.style.display == "none")
            {
                obj.style.display = "block";
                obj2.innerHTML = "Hide Work Instructions";    
            }
            else
            {
                obj.style.display = "none";
                obj2.innerHTML = "View Work Instructions";
            }
         }
    </script>
<table class="raPanelHeader" style="width: 100%; border-bottom: gray 1px solid">
    <tr>
        <td style="width: 100%; cursor: hand; height: 15px; text-align: left">
            <b id="wiLink" onclick="wiHideShow('wiDiv','wiLink');">View Work Instructions</b></td>
    </tr>
</table>
<div id="wiDiv" runat="server" class="raPanel" style="border-top: medium none; display: none">
    <asp:DataList ID="DataList1" runat="server" DataKeyField="WI_ID" DataSourceID="sourceWi">
        <ItemTemplate>
            <asp:LinkButton CausesValidation="False" ID="LinkButton2" runat="server" CommandArgument='<%# Eval("WI_ID") %>'
                OnClick="LinkButton2_Click" Text='<%# Eval("WI_TITLE") %>'></asp:LinkButton><br />
            <br />
        </ItemTemplate>
    </asp:DataList><br />
    <asp:SqlDataSource ID="sourceWi" runat="server" ConnectionString="<%$ ConnectionStrings:IHCConnectionString %>"
        SelectCommand="SELECT WI_ID,WI_TITLE&#13;&#10;FROM WORK_INSTRUCTIONS&#13;&#10;WHERE WI_ID IN&#13;&#10;(SELECT WI_PAGE_ID&#13;&#10;&#9;FROM WORK_INSTRUCTIONS_APP_PAGES&#9;&#13;&#10;&#9;WHERE PAGE_ID = @PAGE_ID)">
        <SelectParameters>
            <asp:Parameter  Name="PAGE_ID" />
        </SelectParameters>
    </asp:SqlDataSource>
</div>
<br />
