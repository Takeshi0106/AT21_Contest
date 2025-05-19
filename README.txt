Unityのバージョンを　2022.3.60f1　に変更しました。

60fps　で実行しています



4/19 更新

https://git-lfs.com/

３Dモデルなどの大きなファイルを扱うためにLFSを導入することにしました。
インストールして
SoureceTreeのターミナルから
以下のコマンドを実行してください

git lfs install


git pull
git lfs pull

をしてください

Git LFS initialized.
が出れば設定完了です。	


メモーーー
ステージのモデルにNavMeshSurfaceの追加とbakeの実行をお願いします。
それと敵のprefab内にNavMeshAgentとChaseScriptが無い場合はこれらを追加しておいてください。
各種値の設定はEnemyStateで行っています。
ー西嶋奨真