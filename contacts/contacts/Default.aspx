<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="contacts.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
        <asp:ListView ID="ContactListView" runat="server"
            ItemType="contacts.Model.Contact"
            SelectMethod="ContactListView_GetDataPageWise"
            DataKeyNames="ContactID"
            InsertMethod="ContactListView_InsertItem"
            UpdateMethod="ContactListView_UpdateItem"
            DeleteMethod="ContactListView_DeleteItem"
            InsertItemPosition="FirstItem">
            <LayoutTemplate>
                <table>
                    <tr>
                        <th>
                            Förnamn
                        </th>
                        <th>
                            Efternamn
                        </th>
                        <th>
                            E-post
                        </th>
                    </tr>

                    <%-- Platshållare för nya rader --%>
                    <asp:PlaceHolder ID="itemPlaceHolder" runat="server" />
                </table>
                <asp:DataPager ID="DataPager1" runat="server" 
                    PageControlID="ContactListView" PageSize="10" >
                    <Fields>
                        <asp:NextPreviousPagerField ButtonType="Button" 
                                                    ShowFirstPageButton="true" 
                                                    ShowNextPageButton="false"
                                                    ShowPreviousPageButton="true"/>

                        <asp:NumericPagerField ButtonType="Link" />
                        
                        <asp:NextPreviousPagerField ButtonType="Button"
                                                    ShowLastPageButton="true"
                                                    ShowNextPageButton="true"
                                                    ShowPreviousPageButton="false" />
                    </Fields>
                </asp:DataPager>
            </LayoutTemplate> 
            <ItemTemplate>
                <%--Mall för nya rader --%>
                <tr>
                    <td>
                        <asp:Label ID="FirstNameLabel" runat="server" Text='<%#: Item.FirstName %>' />
                    </td>
                    <td>
                        <asp:Label ID="LastNameLabel" runat="server" Text='<%#: Item.LastName %>' />
                    </td>
                    <td>
                        <asp:Label ID="EmailLabel" runat="server" Text='<%#: Item.Email %>' />
                    </td>
                    <td>
                        <asp:LinkButton ID="UpdateContact" runat="server" CommandName="Edit" CausesValidation="false" >Redigera</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton ID="DeleteContact" runat="server" CommandName="Delete" CausesValidation="false" OnClientClick='<%# String.Format("return confirm(\"Vill du verkligen ta bort {0} från kontaktlistan?\")", Item.FirstName) %>' >Ta bort</asp:LinkButton>
                    </td>
                </tr>
            </ItemTemplate>
            <EmptyDataTemplate>
                <table>
                    <tr>
                        <td>
                            <%-- Visas om kontaktuppgifter inte existerar i databasen --%>
                        </td>
                    </tr>
                </table>
            </EmptyDataTemplate>
            <InsertItemTemplate>
                <tr>
                    <td>
                        <%--Förnamnsinput --%>
                        <asp:TextBox ID="FirstName" runat="server" Visible="true" Text='<%#:BindItem.FirstName %>' ></asp:TextBox>
                    </td>
                    <td>
                        <%--Efteramnsinput --%>
                        <asp:TextBox ID="LastName" runat="server" Visible="true" Text='<%#:BindItem.LastName %>' ></asp:TextBox>
                    </td>
                    <td>
                        <%--Email-input --%>
                        <asp:TextBox ID="Email" runat="server" Visible="true" Text='<%#:BindItem.Email %>' ></asp:TextBox>
                    </td>
                    <td>
                        <asp:LinkButton CommandName="Insert" runat="server" Text="Lägg till">Lägg till ny</asp:LinkButton>
                    </td>
                    <td>
                        <asp:LinkButton CommandName="Cancel" runat="server" Text="Rensa" CausesValidation="false">Rensa</asp:LinkButton>
                    </td>
                </tr>
            </InsertItemTemplate>
            <EditItemTemplate>
                <tr>
                    <td>
                        <asp:TextBox ID="FirstName" runat="server" Visible="true" Text='<%#: BindItem.FirstName %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="LastName" runat="server" Visible="true" Text='<%#: BindItem.LastName %>' />
                    </td>
                    <td>
                        <asp:TextBox ID="Email" runat="server" Visible="true" Text='<%#: BindItem.Email%>' />
                    </td>
                    <td>
                        <asp:LinkButton CommandName="Update" runat="server" Text="Uppdatera" />
                    </td>
                    <td>
                        <asp:LinkButton CommandName="Cancel" runat="server" Text="Rensa" CausesValidation="false" />
                    </td>
                </tr>
            </EditItemTemplate>
        </asp:ListView>
    </div>
    </form>
</body>
</html>
