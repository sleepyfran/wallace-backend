defmodule Wallace.Repo.Migrations.CreatePayees do
  use Ecto.Migration

  def change do
    create table(:payees) do
      add :name, :string

      timestamps()
    end

  end
end
