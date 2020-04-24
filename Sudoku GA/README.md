Como funciona e como usar o código:
Primeiro, ao iniciar o código o botão de começar a jogar/apagar os números some, e começa o sorteio para ordenar o sudoku.
O processo de ordenação do sudoku, começa com a criação de vetores de 81 posições, onde serão sorteados valores aleatórios, que posteriormente serão transformados em uma matriz de 9x9, que será o sudoku.
Quando criada a matriz, o código executa funções para achar a quantidade de erros presentes nas linhas, colunas e quadrantes(3x3). A forma com que isso é feito, é: percorrer todas as linhas ”X” e as as colunas “Y” e verificar se existem números repetidos nas linhas e colunas. O mesmo vale para os quadrantes, mas a verificação é feita a cada três elementos de cada três linhas e três colunas.
Após essa verificação de erros obteremos um valor total da soma desses três conjuntos de erros. Esse valor será utilizado para calcular a “Fitness” do projeto com a seguinte fórmula: V - Erros = Fitness, sendo “V” um valor aleatório adotado para o que seria a “Fitness perfeita” (onde não há erros).
Já com todos os indivíduos da primeira geração sorteados e a Fitness de cada um deles calculada o código inicia a seleção dos “Parents”, onde o primeiro Parent será escolhido através de uma seleção por torneio, e o segundo Parent será o gene seguinte ao Primeiro parent escolhido.
Depois disso os filhos serão criados a partir de um “Crossover de 2 pontos” com a chance de uma mutação por “swap”. Caso o filhos tenham o melhor
A partir disso, o sudoku vai sendo resolvido de geração por geração. Caso os erros sejam 0, o código de organização para e o jogador tem o botão para começar o jogo, caso demore mais que 2500 gerações para achar e os erros forem menos 6 ou menos (não será entregue o resultado totalmente perfeito, mas existe uma chance altíssima de entregar um resultado jogável), o código para também, liberando o botão para o jogador.

Como usar o código:
Abra a unity, dentro do objeto vazio chamado “sudoku” na cena, o usuário pode configurar como desejar atributos do Algoritmo genético e elementos visuais da unity.
Para realizar um teste “padrão”, o usuário pode colocar os valores no objeto:
Offset: 69
IPosX: 102
IPosY: 74
Text prefab: “Número(Text)”  (Objeto localizado na cena dentro do canvas)  
Canvas: “Canvas” (Canvas já adicionado na cena)
Pop Size: 200
Chromossome Size: 81
Fit Ideal: 1000
Desconto: 1



Link do projeto que foi usado como referência para fazer o sudoku:
https://github.com/erwin-beckers/SudokuGA/tree/master/GAF
