defmodule Wallace.Repo.Migrations.CreateCategories do
  use Ecto.Migration

  def change do
    create table(:categories) do
      add :name, :string
      add :icon, :string
      add :user_id, references(:users, on_delete: :delete_all)

      timestamps()
    end
  end
end
