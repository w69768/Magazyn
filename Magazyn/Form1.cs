using Microsoft.Data.Sqlite;
using System.Data;
using System.Globalization;

namespace Magazyn
{
    public partial class Form1 : Form
    {

        private SqliteConnection connection;
        private ProgramFunctions programFunctions;

        public Form1()
        {
            InitializeComponent();
            connection = new SqliteConnection("Data Source=magazyn.db");
            programFunctions = new ProgramFunctions(connection, listViewProdukty);
            listViewProdukty.Columns.Add("Id", 50, HorizontalAlignment.Left);
            listViewProdukty.Columns.Add("Kod", 150, HorizontalAlignment.Left);
            listViewProdukty.Columns.Add("Nazwa", 200, HorizontalAlignment.Left);
            listViewProdukty.Columns.Add("Iloœæ", 100, HorizontalAlignment.Right);
            listViewProdukty.Columns.Add("Cena", 100, HorizontalAlignment.Right);
            listViewProdukty.FullRowSelect = true;
            listViewProdukty.GridLines = true;
            programFunctions.CreateTable();
            programFunctions.DisplayDataInListView();
        }

        private void buttonIssueProduct_Click(object sender, EventArgs e)
        {
            IssueProduct issueProduct = new IssueProduct(connection, programFunctions.DisplayDataInListView);
            issueProduct.issueProduct();
        }

        private void buttonReceiveProduct_Click(object sender, EventArgs e)
        {
            ReceiveProduct receiveProduct = new ReceiveProduct(connection, programFunctions.DisplayDataInListView);
            receiveProduct.reciveProduct();
        }

        private void buttonSearchProduct_Click(object sender, EventArgs e)
        {
            SearchProduct searchProduct = new SearchProduct(connection);
            searchProduct.searchProduct();
        }

        private void buttonEditProduct_Click(object sender, EventArgs e)
        {
            EditProduct editProduct = new EditProduct(connection, programFunctions.DisplayDataInListView);
            editProduct.editProduct();
        }
    }
}
