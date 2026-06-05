import 'package:flutter/material.dart';

void main() {
  runApp(const AgeCalculatorApp());
}

class AgeCalculatorApp extends StatelessWidget {
  const AgeCalculatorApp({super.key});

  @override
  Widget build(BuildContext context) {
    return MaterialApp(
      debugShowCheckedModeBanner: false,
      title: 'Calculadora de Edad Exacta',
      theme: ThemeData(
        colorScheme: ColorScheme.fromSeed(
          seedColor: Colors.teal,
          brightness: Brightness.light,
        ),
        useMaterial3: true,
        fontFamily: 'Roboto',
      ),
      home: const AgeCalculatorScreen(),
    );
  }
}

class AgeCalculatorScreen extends StatefulWidget {
  const AgeCalculatorScreen({super.key});

  @override
  State<AgeCalculatorScreen> createState() => _AgeCalculatorScreenState();
}

class _AgeCalculatorScreenState extends State<AgeCalculatorScreen> {
  DateTime? _selectedDate;
  String _resultMessage = 'Selecciona tu fecha de nacimiento.';
  String _detailedMessage = '';

  void _presentDatePicker() async {
    final DateTime? pickedDate = await showDatePicker(
      context: context,
      initialDate: DateTime.now().subtract(const Duration(days: 365 * 20)),
      firstDate: DateTime(1900),
      lastDate: DateTime.now(),
      helpText: 'SELECCIONA TU FECHA DE NACIMIENTO',
      cancelText: 'CANCELAR',
      confirmText: 'ACEPTAR',
    );

    if (pickedDate != null) {
      setState(() {
        _selectedDate = pickedDate;
      });
      _calculateExactAge();
    }
  }

  void _calculateExactAge() {
    if (_selectedDate == null) return;

    try {
      final DateTime now = DateTime.now();
      final DateTime birthDate = _selectedDate!;

      if (birthDate.isAfter(now)) {
        throw Exception('La fecha de nacimiento no puede ser en el futuro.');
      }

      int years = now.year - birthDate.year;
      int months = now.month - birthDate.month;
      int days = now.day - birthDate.day;

      if (days < 0) {
        months--;
        final previousMonth = DateTime(now.year, now.month, 0);
        days += previousMonth.day;
      }

      if (months < 0) {
        years--;
        months += 12;
      }

      setState(() {
        _resultMessage = 'Tienes $years años';
        _detailedMessage = '$months meses y $days días';
      });
    } catch (e) {
      String errorMessage = e.toString().replaceFirst('Exception: ', '');
      _showErrorSnackBar(errorMessage);
    }
  }

  void _showErrorSnackBar(String message) {
    ScaffoldMessenger.of(context).showSnackBar(
      SnackBar(
        content: Row(
          children: [
            const Icon(Icons.error_outline, color: Colors.white),
            const SizedBox(width: 8),
            Expanded(child: Text(message)),
          ],
        ),
        backgroundColor: Colors.redAccent,
        behavior: SnackBarBehavior.floating,
        shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(10)),
      ),
    );
  }

  @override
  Widget build(BuildContext context) {
    return Scaffold(
      appBar: AppBar(
        title: const Text('Edad Exacta', style: TextStyle(fontWeight: FontWeight.bold)),
        centerTitle: true,
        backgroundColor: Theme.of(context).colorScheme.primary,
        foregroundColor: Colors.white,
        elevation: 0,
      ),
      body: Container(
        decoration: BoxDecoration(
          gradient: LinearGradient(
            begin: Alignment.topCenter,
            end: Alignment.bottomCenter,
            colors: [
              Theme.of(context).colorScheme.primary.withValues(alpha: 0.1),
              Colors.white,
            ],
          ),
        ),
        child: Padding(
          padding: const EdgeInsets.all(32.0),
          child: Column(
            mainAxisAlignment: MainAxisAlignment.center,
            crossAxisAlignment: CrossAxisAlignment.stretch,
            children: [
              const Icon(
                Icons.calendar_month_rounded,
                size: 100,
                color: Colors.teal,
              ),
              const SizedBox(height: 40),
              Card(
                elevation: 4,
                shape: RoundedRectangleBorder(borderRadius: BorderRadius.circular(16)),
                child: Padding(
                  padding: const EdgeInsets.symmetric(vertical: 24.0, horizontal: 16.0),
                  child: Column(
                    children: [
                      Text(
                        _selectedDate == null 
                          ? 'Ninguna fecha seleccionada'
                          : 'Nacimiento: ${_selectedDate!.day.toString().padLeft(2, '0')}/${_selectedDate!.month.toString().padLeft(2, '0')}/${_selectedDate!.year}',
                        style: const TextStyle(fontSize: 18, color: Colors.black54),
                      ),
                      const SizedBox(height: 20),
                      ElevatedButton.icon(
                        onPressed: _presentDatePicker,
                        icon: const Icon(Icons.edit_calendar),
                        label: const Text('Elegir Fecha', style: TextStyle(fontSize: 18)),
                        style: ElevatedButton.styleFrom(
                          padding: const EdgeInsets.symmetric(vertical: 16, horizontal: 32),
                          shape: RoundedRectangleBorder(
                            borderRadius: BorderRadius.circular(30),
                          ),
                          backgroundColor: Colors.teal,
                          foregroundColor: Colors.white,
                          elevation: 3,
                        ),
                      ),
                    ],
                  ),
                ),
              ),
              const SizedBox(height: 40),
              Text(
                _resultMessage,
                textAlign: TextAlign.center,
                style: TextStyle(
                  fontSize: _selectedDate == null ? 20 : 36,
                  fontWeight: FontWeight.bold,
                  color: Colors.teal.shade800,
                ),
              ),
              if (_detailedMessage.isNotEmpty) ...[
                const SizedBox(height: 8),
                Text(
                  _detailedMessage,
                  textAlign: TextAlign.center,
                  style: const TextStyle(
                    fontSize: 22,
                    fontWeight: FontWeight.w500,
                    color: Colors.black54,
                  ),
                ),
              ]
            ],
          ),
        ),
      ),
    );
  }
}
