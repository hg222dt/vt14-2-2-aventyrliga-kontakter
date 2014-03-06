<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="contacts.Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
    <head runat="server">
        <title>Äventyrliga kontakter</title>
        <link rel="stylesheet" href="css/styles.css" />
    </head>
    <body>
        <form id="form1" runat="server">
            <div>
                <%-- Listview-kontroll för att kunna skriva ut alla kontakter i en tabell --%>
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
                                <asp:NextPreviousPagerField 
                                    ButtonType="Button" 
                                    ShowFirstPageButton="true" 
                                    ShowNextPageButton="false"
                                    ShowPreviousPageButton="true"/>

                                <asp:NumericPagerField 
                                    ButtonType="Link" />
                        
                                <asp:NextPreviousPagerField 
                                    ButtonType="Button"
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
                                <asp:LinkButton 
                                    ID="UpdateContact" 
                                    runat="server" 
                                    CommandName="Edit" 
                                    CausesValidation="false" >Redigera</asp:LinkButton>
                            </td>
                            <td>
                                <asp:LinkButton 
                                    ID="DeleteContact" 
                                    runat="server" 
                                    CommandName="Delete" 
                                    CausesValidation="false" 
                                    OnClientClick='<%# String.Format("return confirm(\"Vill du verkligen ta bort {0} från kontaktlistan?\")", Item.FirstName) %>' >Ta bort</asp:LinkButton>
                            </td>
                        </tr>
                    </ItemTemplate>
                    <EmptyDataTemplate>
                        <table>
                            <tr>
                                <td>
                                    <%-- Visas om kontaktuppgifter inte existerar i databasen --%>
                                    Inga kontaker existerar i databasen.
                                </td>
                            </tr>
                        </table>
                    </EmptyDataTemplate>
                    <InsertItemTemplate>
                        <tr>
                            <td>
                                <%--Förnamnsinput --%>
                                <asp:TextBox ID="InsertFirstNameTB" runat="server" MaxLength="50" Visible="true" Text='<%#:BindItem.FirstName %>' ></asp:TextBox>
                                <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator1" 
                                    runat="server" 
                                    ControlToValidate="InsertFirstNameTB" 
                                    ErrorMessage="Ange förnamn" 
                                    ValidationGroup="InsertGroup" 
                                    Display="None" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <%--Efteramnsinput --%>
                                <asp:TextBox ID="InsertLastNameTB" runat="server" MaxLength="50" Visible="true" Text='<%#:BindItem.LastName %>' ></asp:TextBox>
                                 <asp:RequiredFieldValidator 
                                     ID="RequiredFieldValidator2" 
                                     runat="server" 
                                     ControlToValidate="InsertLastNameTB" 
                                     ErrorMessage="Ange efternamn" 
                                     ValidationGroup="InsertGroup" 
                                     Display="None" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <%--Email-input --%>
                                <asp:TextBox ID="InsertEmailTB" runat="server" Visible="true" Text='<%#:BindItem.Email %>' MaxLength="50"></asp:TextBox>
                                <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator3" 
                                    runat="server" 
                                    ControlToValidate="InsertEmailTB" 
                                    ErrorMessage="Ange e-postadress" 
                                    ValidationGroup="InsertGroup" 
                                    Display="None" ></asp:RequiredFieldValidator>
                                <asp:RegularExpressionValidator 
                                    ID="RegularExpressionValidator1" 
                                    runat="server" 
                                    ControlToValidate="InsertEmailTB" 
                                    ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" 
                                    ErrorMessage="Ange korrekt e-postadress" 
                                    ValidationGroup="InsertGroup" 
                                    Display="None"></asp:RegularExpressionValidator>
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
                                <%-- Kontroller för att editera och validera kontakter. --%>
                                <asp:TextBox ID="UpdateFirstNameTB" runat="server" Visible="true" Text='<%#: BindItem.FirstName %>' MaxLength="50" />
                                <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator1" 
                                    runat="server" 
                                    ControlToValidate="UpdateFirstNameTB" 
                                    ErrorMessage="Ange förnamn" 
                                    ValidationGroup="EditGroup" 
                                    Display="None" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="UpdateLastNameTB" runat="server" Visible="true" Text='<%#: BindItem.LastName %>' MaxLength="50" />
                                <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator2" 
                                    runat="server" 
                                    ControlToValidate="UpdateLastNameTB" 
                                    ErrorMessage="Ange efternamn" 
                                    ValidationGroup="EditGroup" 
                                    Display="None" ></asp:RequiredFieldValidator>
                            </td>
                            <td>
                                <asp:TextBox ID="UpdateEmailTB" runat="server" Visible="true" Text='<%#: BindItem.Email%>' MaxLength="50" />
                                <asp:RequiredFieldValidator 
                                    ID="RequiredFieldValidator3" 
                                    runat="server" 
                                    ControlToValidate="UpdateEmailTB" 
                                    ErrorMessage="Ange e-postadress" 
                                    ValidationGroup="EditGroup" 
                                    Display="None" ></asp:RequiredFieldValidator>

                                <asp:RegularExpressionValidator 
                                    ID="RegularExpressionValidator1" 
                                    runat="server" 
                                    ControlToValidate="UpdateEmailTB" 
                                    ValidationExpression="^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$" 
                                    ErrorMessage="Ange korrekt e-postadress" 
                                    ValidationGroup="EditGroup" 
                                    Display="None" ></asp:RegularExpressionValidator>
                            </td>
                            <td>
                                <asp:LinkButton CommandName="Update" runat="server" Text="Uppdatera" />
                            </td>
                            <td>
                                <asp:LinkButton CommandName="Cancel" runat="server" Text="Avbryt" CausesValidation="false" />
                            </td>
                        </tr>
                    </EditItemTemplate>
                </asp:ListView>
            </div>

            <%-- Felmeddelande --%>
            <asp:Panel ID="statusMessage" runat="server" Visible="False">
                <p>
                    <asp:Label ID="LabelStatusMessage" runat="server" Text="" Visible="false"></asp:Label>
                </p>
                <a href="#" id="CloseLink2">Stäng meddelande</a>
            </asp:Panel>
            
            <%-- Valideringar som utlöses om något blir fel --%>
            <p>
                <asp:ValidationSummary 
                    ID="ValidationSummary1" 
                    runat="server" 
                    HeaderText="Ett fel har inträffat. Gör om, gör rätt!" />
                <asp:ValidationSummary 
                    ID="ValidationSummary2" 
                    runat="server" 
                    HeaderText="Ett fel har inträffat. Gör om, gör rätt!" 
                    ValidationGroup="InsertGroup" 
                    ShowModelStateErrors="false" />
                <asp:ValidationSummary 
                    ID="ValidationSummary3" 
                    runat="server" 
                    HeaderText="Ett fel har inträffat. Gör om, gör rätt!" 
                    ValidationGroup="EditGroup" 
                    ShowModelStateErrors="false" />
            </p>
        </form>

        <%-- Skript ger funktionalitet åt länk att stänga av meddelandet --%>
        <script type="text/javascript">
            setTimeout(function () {
                var closeLink = document.getElementById("CloseLink2");
                var statusDiv = document.getElementById("statusMessage");
                console.log(closeLink);
                closeLink.onclick = function () {
                    statusDiv.parentElement.removeChild(statusDiv);
                };
            }, 1000);
        </script>
    </body>
</html>
