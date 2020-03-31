defmodule Wallace.Repo.Migrations.CreateTransactions do
  use Ecto.Migration

  def change do
    create table(:transactions) do
      add :account_id, references(:accounts, on_delete: :nilify_all)
      add :amount, :money_type
      add :category_id, references(:categories, on_delete: :nilify_all)
      add :date, :utc_datetime
      add :notes, :string
      add :repetition, :string
      add :payee_id, references(:payees, on_delete: :nilify_all)
      add :type_id, references(:transaction_types, on_delete: :nilify_all)

      timestamps()
    end
  end
end
