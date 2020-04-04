defmodule Wallace.Repo.Migrations.CreateAccounts do
  use Ecto.Migration

  def change do
    create table(:accounts) do
      add :name, :string
      add :balance, :money_type
      add :type_id, references(:account_types, on_delete: :nilify_all)
      add :user_id, references(:users, on_delete: :delete_all)

      timestamps()
    end
  end
end
