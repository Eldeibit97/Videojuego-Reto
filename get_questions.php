<?php
$host = 'mysql-2ba70603-tec-18f1.d.aivencloud.com';
$dbname = 'SoluTec';
$port = 18986;
$user = 'avnadmin';
$pass = 'AVNS_Pw5Y7FYmuibFg8VaXaQ';

header('Content-Type: application/json');

try {
    $dsn = "mysql:host=$host;port=$port;dbname=$dbname;charset=utf8mb4";
    $pdo = new PDO($dsn, $user, $pass, [
        PDO::ATTR_ERRMODE => PDO::ERRMODE_EXCEPTION,
        PDO::ATTR_DEFAULT_FETCH_MODE => PDO::FETCH_ASSOC
    ]);
    
    $stmt = $pdo->query("SELECT p.ID_Pregunta, p.Pregunta, r.Correcta FROM Preguntas p JOIN Respuestas r ON p.ID_Pregunta = r.ID_Pregunta");
    $questions = $stmt->fetchAll();
    
    echo json_encode(["questions" => $questions]);
} catch (PDOException $e) {
    echo json_encode(["error" => $e->getMessage()]);
}
?>
