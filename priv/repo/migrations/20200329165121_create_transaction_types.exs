defmodule Wallace.Repo.Migrations.CreateTransactionTypes do
  use Ecto.Migration

  def change do
    create table(:transaction_types) do
      add :name, :string

      timestamps()
    end

  end
end
