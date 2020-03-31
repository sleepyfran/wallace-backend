defmodule Wallace.Repo.Migrations.CreateAccountTypes do
  use Ecto.Migration

  def change do
    create table(:account_types) do
      add :name, :string

      timestamps()
    end

  end
end
