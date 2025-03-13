<?php
$servername = "localhost";
$username = "root";
$password = "4747819";
$dbname = "Problemas_Juego";

// デバッグ用：クエリパラメータの確認
error_log("Query Parameters: " . print_r($_GET, true));

// クエリパラメータからテーブルを取得（デフォルト: problemas_VF）
$table = isset($_GET['table']) ? $_GET['table'] : '';

if ($table !== "problemas_VF" && $table !== "questionario_general") {
    die(json_encode(["error" => "Invalid or missing table name"]));
}

// データベース接続
$conn = new mysqli($servername, $username, $password, $dbname);
if ($conn->connect_error) {
    die(json_encode(["error" => "Connection failed: " . $conn->connect_error]));
}

$questions = array();

if ($table === "problemas_VF") {
    error_log("Fetching data from problemas_VF...");
    $sql = "SELECT id, problema_text, respuesta_correcta FROM problemas_VF";
    $result = $conn->query($sql);
    
    while ($row = $result->fetch_assoc()) {
        $row['id'] = (int)$row['id'];
        $row['respuesta_correcta'] = ($row['respuesta_correcta'] == 1);
        $questions[] = $row;
    }   
} elseif ($table === "questionario_general") {
    error_log("Fetching data from questionario_general...");
    $sql = "SELECT id, problema_text, respuesta_correcta, respuesta_incorrecta1, respuesta_incorrecta2, respuesta_incorrecta3 FROM questionario_general";
    $result = $conn->query($sql);

    while ($row = $result->fetch_assoc()) {
        $row['correct_answer'] = $row['respuesta_correcta'];
        unset($row['respuesta_correcta']);
        $questions[] = $row;
    }
}

// JSON のフォーマットを `{ "questions": [...] }` に統一
echo json_encode(["questions" => $questions], JSON_UNESCAPED_UNICODE);

$conn->close();
?>
