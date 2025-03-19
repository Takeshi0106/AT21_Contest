@echo off
chcp 65001 > nul
setlocal enabledelayedexpansion

REM カラー出力用の関数を定義
call :DEFINE_COLORS

REM タイトル表示
call :LOG_INFO "=============================================="
call :LOG_INFO "      自動フェッチ＆ビルドプロセス 開始       "
call :LOG_INFO "=============================================="
call :LOG_INFO "開始日時: %date% %time%"

REM 1. Git フェッチプロセスを実行
call :LOG_INFO "1. Git フェッチプロセスを開始します..."
call "%~dp0Fetch.bat"

REM フェッチの結果を確認
IF %ERRORLEVEL% NEQ 0 (
    call :LOG_ERROR "Git フェッチプロセスが失敗しました。(エラーコード: %ERRORLEVEL%)"
    call :LOG_ERROR "処理を中止します。"
    pause
    exit /b 1
)

call :LOG_INFO "Git フェッチプロセスが完了しました。"

REM 2. ビルドプロセスを実行
call :LOG_INFO "2. ビルドプロセスを開始します..."
call "%~dp0Deploy.bat"

REM ビルドの結果を確認
IF %ERRORLEVEL% NEQ 0 (
    call :LOG_ERROR "ビルドプロセスが失敗しました。(エラーコード: %ERRORLEVEL%)"
    pause
    exit /b 1
)

call :LOG_SUCCESS "フェッチとビルドの全プロセスが正常に完了しました。"
call :LOG_INFO "終了日時: %date% %time%"

REM 終了メッセージを表示して自動終了
echo 全ての処理を完了しました。ウィンドウは3秒後に自動的に閉じます...
timeout /t 3 > nul
exit

REM ========================================
REM カラー出力用の関数定義
REM ========================================
:DEFINE_COLORS
REM ANSI エスケープシーケンスを使用するための設定
for /F "tokens=1,2 delims=#" %%a in ('"prompt #$H#$E# & echo on & for %%b in (1) do rem"') do (
  set "ESC=%%b"
)
exit /b 0

:LOG_INFO
echo %ESC%[94m[情報]%ESC%[0m %~1
exit /b 0

:LOG_ERROR
echo %ESC%[91m[エラー]%ESC%[0m %~1
exit /b 0

:LOG_SUCCESS
echo %ESC%[92m[成功]%ESC%[0m %~1
exit /b 0