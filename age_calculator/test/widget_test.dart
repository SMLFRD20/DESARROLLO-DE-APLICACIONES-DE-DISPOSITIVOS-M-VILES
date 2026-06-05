import 'package:flutter_test/flutter_test.dart';
import 'package:age_calculator/main.dart';

void main() {
  testWidgets('Calculadora de edad renderiza correctamente', (WidgetTester tester) async {
    await tester.pumpWidget(const AgeCalculatorApp());
    expect(find.text('Edad Exacta'), findsOneWidget);
    expect(find.text('Ninguna fecha seleccionada'), findsOneWidget);
    expect(find.text('Selecciona tu fecha de nacimiento.'), findsOneWidget);
  });
}
