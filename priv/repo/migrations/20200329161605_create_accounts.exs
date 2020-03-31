defmodule Wallace.Repo.Migrations.CreateAccounts do
  use Ecto.Migration

  def change do
    create table(:accounts) do
      add :name, :string
      add :balance, :money_type
      add :type_id, references(:accounts, on_delete: :nilify_all)

      timestamps()
    end
  end
end
