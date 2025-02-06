using Microsoft.Data.Sqlite;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace Magazyn
{
    public class ReceiveProduct
    {
        private readonly SqliteConnection connection;
        private readonly Action displayDataInListView;

        public ReceiveProduct(SqliteConnection connection, Action displayDataInListView)
        {
            this.connection = connection;
            this.displayDataInListView = displayDataInListView;
        }

        public void reciveProduct()
        {
            using (Form receiveForm = new Form())
            {
                receiveForm.Text = "Przyjmij Towar";
                receiveForm.Size = new Size(300, 200);

                Label labelKod = new Label() { Text = "Kod produktu:", Location = new Point(10, 20), AutoSize = true };
                TextBox textBoxKod = new TextBox() { Location = new Point(120, 20), Width = 150 };

                Label labelIlosc = new Label() { Text = "Ilość:", Location = new Point(10, 60), AutoSize = true };
                TextBox textBoxIlosc = new TextBox() { Location = new Point(120, 60), Width = 150 };

                Button buttonOK = new Button() { Text = "OK", Location = new Point(50, 100), DialogResult = DialogResult.OK };
                Button buttonCancel = new Button() { Text = "Anuluj", Location = new Point(150, 100), DialogResult = DialogResult.Cancel };

                receiveForm.Controls.Add(labelKod);
                receiveForm.Controls.Add(textBoxKod);
                receiveForm.Controls.Add(labelIlosc);
                receiveForm.Controls.Add(textBoxIlosc);
                receiveForm.Controls.Add(buttonOK);
                receiveForm.Controls.Add(buttonCancel);

                if (receiveForm.ShowDialog() == DialogResult.OK)
                {
                    string kodProduktu = textBoxKod.Text.Trim();
                    if (string.IsNullOrWhiteSpace(kodProduktu) || !int.TryParse(textBoxIlosc.Text.Trim(), out int ilosc) || ilosc <= 0)
                    {
                        MessageBox.Show("Podaj poprawne dane: kod i ilość.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    ProcessProductReception(kodProduktu, ilosc);
                }
            }
        }

        private void ProcessProductReception(string kodProduktu, int ilosc)
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

                    string updateQuery = "UPDATE Produkty SET Ilość = Ilość + @Ilosc WHERE Id = @Id";
                    SqliteCommand updateCmd = new SqliteCommand(updateQuery, connection);
                    updateCmd.Parameters.AddWithValue("@Ilosc", ilosc);
                    updateCmd.Parameters.AddWithValue("@Id", productId);
                    updateCmd.ExecuteNonQuery();
                    displayDataInListView();
                    MessageBox.Show("Zwiększono ilość produktu.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    reader.Close();
                    AddNewProduct(kodProduktu, ilosc);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd przy przyjmowaniu produktu: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                connection.Close();
            }
        }

        private void AddNewProduct(string kodProduktu, int ilosc)
        {
            using (Form newProductForm = new Form())
            {
                newProductForm.Text = "Nowy Produkt";
                newProductForm.Size = new Size(300, 250);

                Label labelNazwa = new Label() { Text = "Nazwa produktu:", Location = new Point(10, 20), AutoSize = true };
                TextBox textBoxNazwa = new TextBox() { Location = new Point(120, 20), Width = 150 };

                Label labelCena = new Label() { Text = "Cena:", Location = new Point(10, 60), AutoSize = true };
                TextBox textBoxCena = new TextBox() { Location = new Point(120, 60), Width = 150 };

                Button buttonAdd = new Button() { Text = "Dodaj", Location = new Point(50, 100), DialogResult = DialogResult.OK };
                Button buttonCancel = new Button() { Text = "Anuluj", Location = new Point(150, 100), DialogResult = DialogResult.Cancel };

                newProductForm.Controls.Add(labelNazwa);
                newProductForm.Controls.Add(textBoxNazwa);
                newProductForm.Controls.Add(labelCena);
                newProductForm.Controls.Add(textBoxCena);
                newProductForm.Controls.Add(buttonAdd);
                newProductForm.Controls.Add(buttonCancel);

                if (newProductForm.ShowDialog() == DialogResult.OK)
                {
                    string nazwa = textBoxNazwa.Text.Trim();
                    string cenaText = textBoxCena.Text.Trim().Replace('.', ',');
                    if (string.IsNullOrWhiteSpace(nazwa) || !decimal.TryParse(cenaText, out decimal cena) || cena <= 0)
                    {
                        MessageBox.Show("Podaj poprawne dane nowego produktu.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    string insertQuery = "INSERT INTO Produkty (Kod, Nazwa, Ilość, Cena) VALUES (@Kod, @Nazwa, @Ilosc, @Cena)";
                    SqliteCommand insertCmd = new SqliteCommand(insertQuery, connection);
                    insertCmd.Parameters.AddWithValue("@Kod", kodProduktu);
                    insertCmd.Parameters.AddWithValue("@Nazwa", nazwa);
                    insertCmd.Parameters.AddWithValue("@Ilosc", ilosc);
                    insertCmd.Parameters.AddWithValue("@Cena", Math.Round(cena, 2));
                    insertCmd.ExecuteNonQuery();
                    displayDataInListView();
                    MessageBox.Show("Dodano nowy produkt.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
    }
}