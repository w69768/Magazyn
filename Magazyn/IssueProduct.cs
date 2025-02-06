using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magazyn
{
    public class IssueProduct
    {
        private readonly SqliteConnection connection;
        private readonly Action displayDataInListView;

        public IssueProduct(SqliteConnection connection, Action displayDataInListView)
        {
            this.connection = connection;
            this.displayDataInListView = displayDataInListView;
        }

        public void issueProduct()
        {
            using (Form issueForm = new Form())
            {
                issueForm.Text = "Wydaj Produkt";
                issueForm.Size = new Size(300, 200);

                Label labelKod = new Label() { Text = "Kod produktu:", Location = new Point(10, 20), AutoSize = true };
                TextBox textBoxKod = new TextBox() { Location = new Point(120, 20), Width = 150 };

                Label labelIlosc = new Label() { Text = "Ilość do wydania:", Location = new Point(10, 60), AutoSize = true };
                TextBox textBoxIlosc = new TextBox() { Location = new Point(120, 60), Width = 150 };

                Button buttonOK = new Button() { Text = "OK", Location = new Point(50, 100), DialogResult = DialogResult.OK };
                Button buttonCancel = new Button() { Text = "Anuluj", Location = new Point(150, 100), DialogResult = DialogResult.Cancel };

                issueForm.Controls.Add(labelKod);
                issueForm.Controls.Add(textBoxKod);
                issueForm.Controls.Add(labelIlosc);
                issueForm.Controls.Add(textBoxIlosc);
                issueForm.Controls.Add(buttonOK);
                issueForm.Controls.Add(buttonCancel);

                if (issueForm.ShowDialog() == DialogResult.OK)
                {
                    string kodProduktu = textBoxKod.Text.Trim();
                    if (!int.TryParse(textBoxIlosc.Text.Trim(), out int iloscDoWydania) || iloscDoWydania <= 0)
                    {
                        MessageBox.Show("Podaj poprawną ilość do wydania.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    UpdateProductQuantity(kodProduktu, iloscDoWydania);
                }
            }
        }

        private void UpdateProductQuantity(string kodProduktu, int iloscDoWydania)
        {
            try
            {
                connection.Open();
                string selectQuery = "SELECT * FROM Produkty WHERE Kod = @Kod";
                SqliteCommand selectCmd = new SqliteCommand(selectQuery, connection);
                selectCmd.Parameters.AddWithValue("@Kod", kodProduktu);

                SqliteDataReader reader = selectCmd.ExecuteReader();
                if (reader.Read())
                {
                    int currentQuantity = Convert.ToInt32(reader["Ilość"]);
                    int productId = Convert.ToInt32(reader["Id"]);
                    reader.Close();

                    if (iloscDoWydania > currentQuantity)
                    {
                        MessageBox.Show("Nie można wydać więcej, niż jest w magazynie. W magazynie jest: " + currentQuantity, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                    else if (iloscDoWydania == currentQuantity)
                    {
                        string deleteQuery = "DELETE FROM Produkty WHERE Id = @Id";
                        SqliteCommand deleteCmd = new SqliteCommand(deleteQuery, connection);
                        deleteCmd.Parameters.AddWithValue("@Id", productId);
                        deleteCmd.ExecuteNonQuery();
                        displayDataInListView();
                        MessageBox.Show("Produkt został wydany. Pozostała ilość to 0. Usunięto z magazynu.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        string updateQuery = "UPDATE Produkty SET Ilość = Ilość - @Ilosc WHERE Id = @Id";
                        SqliteCommand updateCmd = new SqliteCommand(updateQuery, connection);
                        updateCmd.Parameters.AddWithValue("@Ilosc", iloscDoWydania);
                        updateCmd.Parameters.AddWithValue("@Id", productId);
                        updateCmd.ExecuteNonQuery();
                        displayDataInListView();
                        MessageBox.Show("Produkt został wydany.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
                else
                {
                    MessageBox.Show("Nie znaleziono produktu o podanym kodzie.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd przy wydawaniu produktu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
