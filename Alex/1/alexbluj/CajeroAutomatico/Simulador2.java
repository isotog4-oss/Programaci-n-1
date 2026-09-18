public class Simulador2
{
    static public void main(String[] args)
    {
        Banco banco =  new  Banco("Banco Universitario");
        
        Cliente cliente1 =  new  Cliente("Ana López", "1001");
        Cliente cliente2 =  new  Cliente("Carlos Pérez", "1002");
        Cliente cliente3 =  new  Cliente("María Gómez", "1003");
        
        CuentaBancaria cuenta1 =  new  CuentaBancaria("1001", cliente1, 2000.00);
        CuentaBancaria cuenta2 =  new  CuentaBancaria("1002", cliente2, 1200.00);
        CuentaBancaria cuenta3 =  new  CuentaBancaria("1003", cliente3, 500.00);
        
        Tarjeta tarjeta1 =  new  Tarjeta("TAR-001", "1234", cuenta1);
        Tarjeta tarjeta2 =  new  Tarjeta("TAR-002", "2345", cuenta2);
        Tarjeta tarjeta3 =  new  Tarjeta("TAR-003", "3456", cuenta3);
        
        banco.registrarTarjeta(tarjeta1);
        banco.registrarTarjeta(tarjeta2);
        banco.registrarTarjeta(tarjeta3);
        banco.registrarCuenta(cuenta1);
        banco.registrarCuenta(cuenta2);
        banco.registrarCuenta(cuenta3);
        CajeroAutomatico cajero =  new  CajeroAutomatico(banco);
        
        System.out.println("Intento con PIN incorrecto: " + cajero.autenticar("TAR-001", "0000"));
        
        System.out.println(cajero.depositar(100.00));
        
        if (cajero.autenticar("TAR-001", "1234")) {
            System.out.println("Saldo Ana: " + cajero.consultarSaldo());
            System.out.println(cajero.depositar(300.00));
            System.out.println(cajero.transferir("1003", 700.00));
            System.out.println("Saldo final Ana: " + cajero.consultarSaldo());
        }
        
        if (cajero.autenticar("TAR-002", "2345")) {
            System.out.println(cajero.retirar(400.00));
            System.out.println(cajero.transferir("1001", 250.00));
        }
        
        if (cajero.autenticar("TAR-003", "3456")) {
            System.out.println(cajero.retirar(1500.00));
        }
        mostrarHistorial(cuenta1);
        mostrarHistorial(cuenta2);
        mostrarHistorial(cuenta3);
    }

    static private void mostrarHistorial(CuentaBancaria cuenta)
    {
        System.out.println("\nHistorial de la cuenta " + cuenta.getNumero() + ":");
        for (final Transaccion transaccion : cuenta.getHistorial()) {
            System.out.println("- " + transaccion.getTipo() + " por Q" + transaccion.getMonto() + " el " + transaccion.getFecha());
        }
    }
}
